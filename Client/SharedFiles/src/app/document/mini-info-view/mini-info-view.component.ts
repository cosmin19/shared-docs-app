import { Component, OnInit } from '@angular/core';
import { DocumentMiniInfo } from '../../_models/document/document-mini-info';
import { Router } from '@angular/router';
import { AlertService } from '../../_services';
import { DocumentService } from '../../_services/document.service';

@Component({
  selector: 'app-mini-info-view',
  templateUrl: './mini-info-view.component.html',
  styleUrls: ['./mini-info-view.component.css']
})
export class MiniInfoViewComponent implements OnInit {

    documents: DocumentMiniInfo[];

    constructor(
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService,
        
    ) { }

    ngOnInit() {
        this._documentService.listDocumentsView()
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
        this._documentService.listDocumentsView()
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
}
