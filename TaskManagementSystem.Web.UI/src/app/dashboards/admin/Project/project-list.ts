import { ChangeDetectorRef,Component,OnInit } from "@angular/core";
import { ProjectService } from '../../../services/project.service';
import { ProjectResponseDto } from '../../../models/project.model'
import { CommonModule } from "@angular/common";
import { SpinnerLoadingService } from "../../../services/spinner-loading-service";

@Component({
    imports: [CommonModule],
    selector: 'app-project-list',
    styleUrl: './project-list.css',
    templateUrl: './project-list.html',
})
export class ProjectListComponent implements OnInit{
    isLoading = false;

    projects: ProjectResponseDto[]=[];

    constructor(
        private projectService: ProjectService,
        private spinner: SpinnerLoadingService,
        private cdr:ChangeDetectorRef){ }

        ngOnInit(): void {
            this.isLoading = true;
            this.spinner.show('loading projects............');
            this.projectService.getProjectsAsync().subscribe({
                next: (data) => {
                    this.projects = data,
                    this.isLoading = false,
                    this.spinner.hide(),
                    this.cdr.detectChanges();
                },
                error: (error) => {
                    console.error('Error Ftching projects:',error);
                    this.isLoading = false,
                    this.spinner.hide(),
                    this.cdr.detectChanges();
                }
            });
        }
        editProject(projectId:number):void{
            console.log(`Edit projects with ID: ${projectId}`);
        }
}