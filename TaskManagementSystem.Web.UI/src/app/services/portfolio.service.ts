import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { PortfolioResponseDto } from "../models/portfoilio.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class PortfolioService {
    constructor(private apiService: ApiService) { }

    getPortfoliosAsync(): Observable<PortfolioResponseDto[]> {
        //https://localhost:7011/api/portfolios
        return this.apiService.send<PortfolioResponseDto[]>("GET","portfolios");
    }
}
