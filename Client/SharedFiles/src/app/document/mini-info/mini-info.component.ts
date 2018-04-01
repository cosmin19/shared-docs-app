import { Component, OnInit, Input } from '@angular/core';
import { DocumentMiniInfo } from '../../_models/document/document-mini-info';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { DocumentService } from '../../_services/document.service';
import { Router } from '@angular/router';
import { AlertService } from '../../_services';

@Component({
    selector: 'document-mini-info',
    templateUrl: './mini-info.component.html',
    styleUrls: ['./mini-info.component.css']
})
export class MiniInfoComponent implements OnInit {

    @Input() document: DocumentMiniInfo;

    constructor(
        private router: Router,
        private _alertService: AlertService,
        private confirmationService: ConfirmationService,
        private _documentService: DocumentService
    ) { }

    ngOnInit() {

    }



}
