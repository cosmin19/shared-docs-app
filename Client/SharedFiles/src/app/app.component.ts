import { Component } from '@angular/core';
import { Message } from 'primeng/components/common/api';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    msgs: Message[] = [];

    constructor() { }
}
