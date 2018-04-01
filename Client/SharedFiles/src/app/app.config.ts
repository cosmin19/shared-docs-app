import { Injectable } from '@angular/core';
@Injectable()
export class AppConfig {
    baseUrl:string = 'http://localhost:60979/';


    loadingGifUrl: string = 'assets/content/loading.gif';
    tagLogoUrl: string = 'assets/content/logo/tag-logo.png';
}