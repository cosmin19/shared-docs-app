import { Component, OnInit } from '@angular/core';
import { DocumentMiniInfo } from '../_models/document/document-mini-info';
import { Router } from '@angular/router';
import { AlertService } from '../_services';
import { DocumentService } from '../_services/document.service';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { DocumentEditDto } from '../_models/document/document-edit';

@Component({
    selector: 'app-document',
    templateUrl: './document.component.html',
    styleUrls: ['./document.component.css']
})
export class DocumentComponent implements OnInit {

    documents: DocumentMiniInfo[];
    editDocument: DocumentEditDto = new DocumentEditDto();
    renderEditModal: boolean = false;

    constructor(
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService,
        private confirmationService: ConfirmationService
        
    ) { }

    ngOnInit() {
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

    view() {

    }

    edit(event) {
        this.renderEditModal = false;
        this.editDocument = event;
        this.renderEditModal = true;
    }

    delete(event) {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to delete this document?',
            accept: () => {
                this._documentService.deleteDocument(event)
                .subscribe(
                    data => { 
                        if(data.success){
                            let message = data.message;
                            this._alertService.success("Document", message);
                            this.refresh();
                        }
                        else {
                            let message = data.message;
                            this._alertService.error("Document", message);
                        }
                    },
                    error => {
                        let message = error.error.message;
                        this._alertService.error("Document", message);
                    }
                );
            }
        });
    }

    documentEmitAction(event: boolean) {
        console.log("aj aici");
        if(event)
            this.refresh();
    }

}
