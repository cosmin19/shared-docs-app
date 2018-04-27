import { Component, OnInit } from '@angular/core';
import { AuthenticationService, AlertService } from '../../_services';
import { DocumentService } from '../../_services/document.service';

@Component({
    selector: 'user-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {


    constructor(private _authService: AuthenticationService) { }

    ngOnInit() {

    }

    logOut() {
        this._authService.logout();
    }

}
