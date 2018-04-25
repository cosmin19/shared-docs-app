import { Component, OnInit } from '@angular/core';
import { DocumentMiniInfo } from '../../_models/document/document-mini-info';
import { DocumentEditDto } from '../../_models/document/document-edit';
import { AlertService } from '../../_services';
import { DocumentService } from '../../_services/document.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-mini-info-edit',
    templateUrl: './mini-info-edit.component.html',
    styleUrls: ['./mini-info-edit.component.css']
})
export class MiniInfoEditComponent implements OnInit {

    documents: DocumentMiniInfo[];
    editDocument: DocumentEditDto = new DocumentEditDto();
    renderEditModal: boolean = false;

    constructor(
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService,

    ) { }

    ngOnInit() {
        this._documentService.listDocumentsEdit()
            .subscribe(
                data => {
                    this.documents = data;
                },
                error => {
                    let message = error.error.message;
                    this._alertService.error("Document", message);
                }
            );
    }

    refresh() {
        this.documents = [];
        this._documentService.getAllDocumentsForUser()
            .subscribe(
                data => {
                    this.documents = data;
                },
                error => {
                    let message = error.error.message;
                    this._alertService.error("Document", message);
                }
            );
    }

    edit(event) {
        this.renderEditModal = false;
        this.editDocument = event;
        this.renderEditModal = true;
    }
}
