package com.verification.service;

import com.fasterxml.jackson.core.type.TypeReference;
import com.verification.dto.request.VerifyCardRequest;
import com.verification.dto.response.BaseResponse;
import com.verification.dto.response.LoginResponse;
import com.verification.dto.response.SecretKeyResponse;
import com.verification.dto.response.VerifyCardResponse;
import com.verification.utils.JsonUtils;
import com.verification.utils.VerificationUtils;
import lombok.*;
import lombok.extern.slf4j.Slf4j;
import okhttp3.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class VerificationService {
    private String url;
    public BaseResponse<VerifyCardResponse> verify(String apiKey, String secretKey, String transactionId, long timestamp, VerifyCardRequest verifyCardRequest) {
        try {
            OkHttpClient client = new OkHttpClient();

            // tạo hash để auth giữa client và server
            String hash = VerificationUtils.buildRequestHash(apiKey, secretKey, transactionId, timestamp, verifyCardRequest.getDg1DataB64());
            HttpUrl.Builder urlBuilder = HttpUrl.parse(url + "/api/v1/c06-verify/integration/verify-card").newBuilder();
            urlBuilder.addQueryParameter("apiKey", apiKey);
            urlBuilder.addQueryParameter("transactionId", transactionId);
            urlBuilder.addQueryParameter("timestamp", String.valueOf(timestamp));
            urlBuilder.addQueryParameter("hash", hash);
            String bodyData = JsonUtils.toJson(verifyCardRequest);
            RequestBody requestBody = RequestBody.create(MediaType.parse("application/json"), bodyData);
            Request request = new Request.Builder()
                    .url(urlBuilder.build())
                    .post(requestBody)
                    .build();
            Response response = client.newCall(request).execute();
            String responseData = response.body().string();
            BaseResponse<VerifyCardResponse> verifyCardResponseBaseResponse = JsonUtils.fromJson(responseData,new TypeReference<>() {
            });
            return  verifyCardResponseBaseResponse;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public BaseResponse<LoginResponse> login(String username, String password) {
        try {
            OkHttpClient client = new OkHttpClient();
            HttpUrl.Builder urlBuilder = HttpUrl.parse(url + "/api/v1/auth/customer/login").newBuilder();
            RequestBody formBody = new FormBody.Builder()
                    .add("username", username)
                    .add("password", password)
                    .build();
            Request request = new Request.Builder()
                    .addHeader("Content-Type", "application/x-www-form-urlencoded")
                    .url(urlBuilder.build())
                    .post(formBody)
                    .build();
            Response response = client.newCall(request).execute();
            String responseData = response.body().string();
            BaseResponse<LoginResponse> loginResponseBaseResponse = JsonUtils.fromJson(responseData,new TypeReference<>() {
            });
            return  loginResponseBaseResponse;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public BaseResponse<SecretKeyResponse> getSecretKey(String accessToken) {
        try {
            OkHttpClient client = new OkHttpClient();
            HttpUrl.Builder urlBuilder = HttpUrl.parse(url + "/api/v1/customers/me/api-key").newBuilder();
            Request request = new Request.Builder()
                    .addHeader("Authorization", "Bearer " + accessToken)
                    .url(urlBuilder.build())
                    .get()
                    .build();
            Response response = client.newCall(request).execute();
            String responseData = response.body().string();
            BaseResponse<SecretKeyResponse> secretKeyResp = JsonUtils.fromJson(responseData,new TypeReference<>() {
            });
            return secretKeyResp;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public BaseResponse<SecretKeyResponse> resetSecretKey(String accessToken, String apiKey) {
        try {
            OkHttpClient client = new OkHttpClient();
            HttpUrl.Builder urlBuilder = HttpUrl.parse(url + "/api/v1/customers/me/api-key/reset").newBuilder();
            RequestBody formBody = new FormBody.Builder()
                    .add("apiKey", apiKey)
                    .build();
            Request request = new Request.Builder()
                    .addHeader("Authorization", "Bearer " + accessToken)
                    .url(urlBuilder.build())
                    .post(formBody)
                    .build();
            Response response = client.newCall(request).execute();
            String responseData = response.body().string();
            BaseResponse<SecretKeyResponse> secretKeyResp = JsonUtils.fromJson(responseData,new TypeReference<>() {
            });
            return secretKeyResp;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }
}
