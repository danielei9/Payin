package com.mobile.application.payin.dto.results;

import java.util.ArrayList;

public class WorkerGetAllResult {
    public ArrayList<Worker> Data;

    public WorkerGetAllResult() {
        Data = new ArrayList<>();
    }

    public static class Worker {
        public int    Id;
        public String ConcessionName;
        public int    State;
        public int    Type;

        public Worker() {
        }
    }
}
