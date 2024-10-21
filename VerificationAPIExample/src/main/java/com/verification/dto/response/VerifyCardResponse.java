package com.verification.dto.response;

import lombok.*;

@Setter
@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class VerifyCardResponse {
    private boolean result;
    private long timestamp;
    private String signature;
}
