import { Component, OnInit, Input } from '@angular/core';
import { DocumentService } from '../../_services/document.service';
import { AlertService } from '../../_services';
import { Router } from '@angular/router';
import { UserRecordDto } from '../../_models/user-record';
import { OtherClientDocumentIdsDto, InvitationActionType } from '../../_models/other-client-document';

@Component({
    selector: 'app-all-users',
    templateUrl: './all-users.component.html',
    styleUrls: ['./all-users.component.css']
})
export class AllUsersComponent implements OnInit {

    users: UserRecordDto[];
    @Input() documentId: number;
    constructor(
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService,

    ) { }

    ngOnInit() {
        this._documentService.getAllUsers()
            .subscribe(
                data => {
                    this.users = data;
                },
                error => {
                    let message = error.error.message;
                    this._alertService.error("Users", message);
                }
            );
    }

    refresh() {
        this.users = [];
        this._documentService.getAllUsers()
            .subscribe(
                data => {
                    this.users = data;
                },
                error => {
                    let message = error.error.message;
                    this._alertService.error("Users", message);
                }
            );
    }

    inviteToView(userId: number) {
        let model = new OtherClientDocumentIdsDto();
        model.ActionType = InvitationActionType.View;
        model.DocumentId = this.documentId;
        model.ClientId = userId;
        this._documentService.inviteClient(model)
            .subscribe(
                data => {
                    if (data.success)
                        this._alertService.success("Invitaion", data.message);
                    else
                        this._alertService.error("Invitaion", data.message);
                },
                error => {
                    let message = error.error.message;
                    this._alertService.error("Users", message);
                }
            );
    }

    inviteToEdit(userId: number) {
        let model = new OtherClientDocumentIdsDto();
        model.ActionType = InvitationActionType.Edit;
        model.DocumentId = this.documentId;
        model.ClientId = userId;

        this._documentService.inviteClient(model)
            .subscribe(
                data => {
                    this._alertService.info("Invitaion", data.message);
                },
                error => {
                    let message = error.error.message;
                    this._alertService.error("Users", message);
                }
            );
    }
}
