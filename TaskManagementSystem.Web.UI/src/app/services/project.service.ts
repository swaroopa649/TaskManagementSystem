import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { ProjectResponseDto } from "../models/project.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class ProjectService{
    constructor(private apiService: ApiService){}

    getProjectsAsync(): Observable<ProjectResponseDto[]>{
        return this.apiService.send<ProjectResponseDto[]>("GET","projects");
    }
}