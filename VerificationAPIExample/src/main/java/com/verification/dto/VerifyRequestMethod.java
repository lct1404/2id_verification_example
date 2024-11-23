package com.verification.dto;

public enum VerifyRequestMethod {
    C06("C06", "C06"), INTEGRITY("INTEGRITY", "Toàn vẹn dữ liệu");

    private String value;
    private String desc;

    VerifyRequestMethod(String value, String desc) {
        this.value = value;
        this.desc = desc;
    }
}
