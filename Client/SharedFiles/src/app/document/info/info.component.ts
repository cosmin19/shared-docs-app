import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from '../../_services';
import { DocumentInfoDto } from '../../_models/document/document-info';
import { DocumentService } from '../../_services/document.service';
import { ClientDto } from '../../_models/client';
import { OtherClientDocumentIdsDto, InvitationActionType } from '../../_models/other-client-document';

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
        this.refresh();
    }

    refresh() {
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

    kickUserFromView(clientId: number) {
        let model = new OtherClientDocumentIdsDto();
        model.ActionType = InvitationActionType.View;
        model.DocumentId = this.documentId;
        model.ClientId = clientId;
        
        this._documentService.kickViewDocument(model)
        .subscribe(
            data => {
                if(data.success) {
                    this._alertService.success("Viewer", data.message);
                    this.refresh();
                }
                else
                    this._alertService.error("Viewer", "Error")
            },
            error => {
                this._alertService.error("Viewer", "Error")
            }
        )
    }

    kickUserFromEdit(clientId: number) {
        let model = new OtherClientDocumentIdsDto();
        model.ActionType = InvitationActionType.Edit;
        model.DocumentId = this.documentId;
        model.ClientId = clientId;
        
        this._documentService.kickEditDocument(model)
        .subscribe(
            data => {
                if(data.success) {
                    this._alertService.success("Editer", data.message);
                    this.refresh();
                }
                else
                    this._alertService.error("Editer", "Error")
            },
            error => {
                this._alertService.error("Editer", "Error")
            }
        )
    }
}
