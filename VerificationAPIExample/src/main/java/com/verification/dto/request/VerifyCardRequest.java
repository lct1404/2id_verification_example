package com.verification.dto.request;

import lombok.*;

@Setter
@Getter
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class VerifyCardRequest {
    private String dg1DataB64;
    private String dg2DataB64;
    private String dg13DataB64;
    private String dg14DataB64;
    private String sodData;
    private String fileId;
}
