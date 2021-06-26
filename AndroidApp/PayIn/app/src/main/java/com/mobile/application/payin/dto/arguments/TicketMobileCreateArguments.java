package com.mobile.application.payin.dto.arguments;

import java.io.Serializable;

public class TicketMobileCreateArguments implements Serializable {
    public String Reference;
    public String Title;
    public String Date;
    public Double Amount;
    public int    ConcessionId;

    public TicketMobileCreateArguments() {
    }
}
