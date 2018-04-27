import { Injectable } from '@angular/core';
@Injectable()
export class AppConfig {
    baseUrl:string = 'http://localhost:64305/';


    loadingGifUrl: string = 'assets/content/loading.gif';
    tagLogoUrl: string = 'assets/content/logo/tag-logo.png';
}