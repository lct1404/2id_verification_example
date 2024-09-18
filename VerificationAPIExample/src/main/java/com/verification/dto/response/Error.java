package com.verification.dto.response;

import lombok.*;

@Setter
@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Error {
    private Object code;
    private Message message;
}

@Setter
@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
class Message {
    private String vn;
    private String en;
}
