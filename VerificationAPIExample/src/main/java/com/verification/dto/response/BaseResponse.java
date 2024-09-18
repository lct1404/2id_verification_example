package com.verification.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.*;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class BaseResponse<T> {
    private boolean status;
    private String timestamp;
    private String transId;
    private T data;
    private Error[] errors;
}
