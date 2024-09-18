package com.verification.dto.response;

import lombok.*;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class SecretKeyResponse {
    private String key;
    private String secretKey;
    private String createdAt;
    private String createdBy;
    private String updatedBy;
}