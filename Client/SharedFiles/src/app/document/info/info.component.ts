import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from '../../_services';
import { DocumentInfoDto } from '../../_models/document/document-info';
import { DocumentService } from '../../_services/document.service';
import { ClientDto } from '../../_models/client';

@Component({
    selector: 'app-info',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.css']
})
export class DocumentInfoComponent implements OnInit {

    documentId: number;
    document: DocumentInfoDto;
    viewers: ClientDto[];
    editers: ClientDto[];

    constructor (
        private router: Router,
        private route: ActivatedRoute,
        private _documentService: DocumentService, 
        private _alertService: AlertService) {

        this.documentId = Number(this.route.snapshot.params['id']);
        if (isNaN(this.documentId))
            this.router.navigate(['']);
    }

    ngOnInit() {
        this._documentService.getDocumentById(this.documentId)
        .subscribe(
            data => {
                this.document = data;
                if(data.isOwnerDocument) {
                    this.prepareViewers();
                    this.prepareEditers();
                }
            },
            error => {
                let message = error.error.message;
                this._alertService.error("Document", message);
            }
        );
    }

    prepareViewers() {
        this._documentService.getViewersForDocument(this.document.id)
        .subscribe(
            data => {
                this.viewers = data;
            },
            error => {
                this._alertService.error("Viewers", "Error")
            }

        )
    }

    prepareEditers() {
        this._documentService.getEditersForDocument(this.document.id)
        .subscribe(
            data => {
                this.editers = data;
            },
            error => {
                this._alertService.error("Editers", "Error")
            }

        )
    }


}
