import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from '../../_services';
import { DocumentService } from '../../_services/document.service';
import { NotificationDto } from '../../_models/notification';

@Component({
    selector: 'app-notifications',
    templateUrl: './notifications.component.html',
    styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {

    notifications: NotificationDto[];
    constructor(
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService,
        
    ) { }

    ngOnInit() {
        this._documentService.getNotifications()
        .subscribe(
            data => {
                this.notifications = data;
            },
            error => {
                let message = error.error.message;
                this._alertService.error("Notifications", message);
            }
        );
    }

    refresh() {
        this.notifications = [];
        this._documentService.getNotifications()
        .subscribe (
            data => {
                this.notifications = data;
                console.log(this.notifications);
            },
            error => {
                let message = error.error.message;
                this._alertService.error("Notifications", message);
            }
        );
    }

    acceptInvitation(invitationId: number) {
        this._documentService.acceptInvitation(invitationId)
        .subscribe
        (
            data => {
                this._alertService.success("Notifications", data.message);
                this.refresh();
            },
            error => {
                let message = error.error.message;
                this._alertService.error("Notifications", message);
            }
        );
    }

    
}
