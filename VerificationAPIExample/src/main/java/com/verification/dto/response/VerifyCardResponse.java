package com.verification.dto.response;

import lombok.*;

@Setter
@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class VerifyCardResponse {
    private Boolean result;
    private Long time;
    private String signature;
    private String responseId;
    private String message;
    private Integer exitcode;
}
