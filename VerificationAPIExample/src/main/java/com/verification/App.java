package com.verification;

import com.verification.dto.VerifyRequestMethod;
import com.verification.dto.request.VerifyCardRequest;
import com.verification.dto.response.BaseResponse;
import com.verification.dto.response.LoginResponse;
import com.verification.dto.response.SecretKeyResponse;
import com.verification.dto.response.VerifyCardResponse;
import com.verification.service.VerificationService;

import java.util.UUID;

public class App 
{
    public static void main( String[] args )
    {
        String url = "https://api-c06verify.2id.vn";
        String apiKey = "1d37e913-b2ce-4249-b4df-703c5ac18309";
        String secretKey = "2a573c48321381a7ae1f44cefbf69cda";
        VerificationService verificationService = new VerificationService(url);
        String transactionId = UUID.randomUUID().toString();
        long timestamp = System.currentTimeMillis();
        String verifyRequestMethod = VerifyRequestMethod.C06.name();
//        String verifyRequestMethod = VerifyRequestMethod.INTEGRITY.name();
        VerifyCardRequest request = VerifyCardRequest.builder()
                .dg1DataB64("YV1fH1pJRFZOTTA5OTAwODA0MzkwMzcwOTkwMDgwNDM8PDU5OTExMDk3TTI0MTEwOTNWTk08PDw8PDw8PDw8PDZESU5IPDxDT05HPFNPTjw8PDw8PDw8PDw8PDw8PDw=")
                .dg2DataB64("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")
                .dg13DataB64("bYIBnjCCAZoCAQEGBijTFgEACDGCAYswEQIBARMMMDM3MDk5MDA4MDQzMBUCAQIMEMSQaW5oIEPDtG5nIFPGoW4wDwIBAxMKMDkvMTEvMTk5OTAIAgEEDANOYW0wDwIBBQwKVmnhu4d0IE5hbTAJAgEGDARLaW5oMAsCAQcMBktow7RuZzAmAgEIDCFHaWEgSMawbmcsIEdpYSBWaeG7hW4sIE5pbmggQsOsbmgwPQIBCQw4VGjDtG4gUXV54bq/dCBUaOG6r25nLCBYw61jaCBUaOG7lSwgTmhvIFF1YW4sIE5pbmggQsOsbmgwNQIBCgwwTuG7kXQgcnXhu5NpIEM6IDNjbSBkxrDhu5tpIHNhdSBjw6FuaCBtxalpIHRyw6FpMA8CAQsTCjI5LzA5LzIwMjIwDwIBDAwKMDkvMTEvMjAyNDArAgENMBMMEcSQaW5oIEPDtG5nIEzDqm5oMBEMD0jDoCBUaOG7iyBMw6FuaDAHAgEOMAIMADAOAgEPEwkxNjQ2NTMzODQwFQIBEBMQMDI5NDhCNURGRERCMDAwMA==")
                .dg14DataB64("boIBfjGCAXowDQYIBAB/AAcCAgICAQEwDwYKBAB/AAcCAgMCAgIBATASBgoEAH8ABwICBAICAgECAgENMIIBQgYJBAB/AAcCAgECMIIBMzCB7AYHKoZIzj0CATCB4AIBATAsBgcqhkjOPQEBAiEAqftX26Huqbw+ZgqQnYONcm479iPVJiAoIBNIHR9uU3cwRAQgfVoJdfwsMFfu9nUwQXr/5/uAVcEm3Fxs6UpLRPMwtdkEICbcXGzpSktE8zC12bvXfL+VhBYpXPfhzmvM3Bj/jAe2BEEEi9Kuuct+V8ssS0gv/IG3r7neJ+HjvSPCOkRTvZrOMmJUfvg1w9rE/Zf4RhoUYR3JwndFEy3tjlRcHVTHLwRplwIhAKn7V9uh7qm8PmYKkJ2DjXGMOXqjtWGm95AeDoKXSFanAgEBA0IABDSrVdzQ/fmse7+10cycVQIOvp2KIm1Gf4mM1UjfnTrkIb1cD+JVPhtf0afmjWgcPUDIEOoJmp0ahRMJAaYBa+k=")
                .sodData("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")
                .method(verifyRequestMethod)
                .build();
        BaseResponse<VerifyCardResponse> verifyCardResponseBaseResponse = verificationService.verify(apiKey, secretKey, transactionId, timestamp, request);
        if (!verifyCardResponseBaseResponse.isStatus()) {
            System.out.println("Verify failed!");
            System.out.println(verifyCardResponseBaseResponse.getErrors()[0].getCode());
            return;
        }
        VerifyCardResponse verifyCardResponse = verifyCardResponseBaseResponse.getData();
        System.out.println("Verify: " + verifyCardResponse.getResult()); // kết quả xác thực từ BCA
        System.out.println("time: " + verifyCardResponse.getTime()); // thời gian xác thực từ BCA
        System.out.println("Signature: " + verifyCardResponse.getSignature()); // chữ ký số của BCA
        System.out.println("exitcode: " + verifyCardResponse.getExitcode()); // Trạng thái yêu cầu xác thực
        System.out.println("message: " + verifyCardResponse.getMessage()); // Thông báo yêu cầu xác thực
        System.out.println("responseId: " + verifyCardResponse.getResponseId()); // Mã yêu cầu xác thực
    }
}
