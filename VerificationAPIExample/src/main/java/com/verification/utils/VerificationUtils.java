package com.verification.utils;

import lombok.extern.slf4j.Slf4j;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.util.HexFormat;

public class VerificationUtils {

    public static String buildRequestHash(String apiKey, String secretKey, String transactionId, long timestamp, String message) {
        String verifyHashPlainText = buildHashValue(apiKey, transactionId, timestamp, message);
        String verifyHash = hashSHA512(secretKey, verifyHashPlainText);
        return HexFormat.of().formatHex(verifyHash.getBytes());
    }

    private static String buildHashValue(String apiKey, String transactionId, long timestamp, String message) {
        return apiKey + transactionId + timestamp + message;
    }

    public static String hashSHA512(final String key, final String data) {
        try {
            if (key == null || data == null) throw new NullPointerException();

            final Mac hmac512 = Mac.getInstance("HmacSHA512");
            byte[] hmacKeyBytes = key.getBytes();
            final SecretKeySpec secretKey = new SecretKeySpec(hmacKeyBytes, "HmacSHA512");
            hmac512.init(secretKey);
            byte[] dataBytes = data.getBytes(StandardCharsets.UTF_8);
            byte[] result = hmac512.doFinal(dataBytes);
            StringBuilder sb = new StringBuilder(2 * result.length);
            for (byte b : result) {
                sb.append(String.format("%02x", b & 0xff));
            }
            return sb.toString();
        } catch (Exception ex) {
            return "";
        }
    }
}
