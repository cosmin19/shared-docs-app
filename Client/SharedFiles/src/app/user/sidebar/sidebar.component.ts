import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../_services';

@Component({
    selector: 'user-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],

})
export class SidebarComponent implements OnInit {

    constructor(private _authService: AuthenticationService) {
    }
    ngOnInit() {
    
    }

    logOut() {
        this._authService.logout();
    }
}
