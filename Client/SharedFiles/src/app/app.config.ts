import { Injectable } from '@angular/core';
@Injectable()
export class AppConfig {
    // baseUrl:string = 'https://141.85.224.158/';  
    // baseUrl:string = 'https://localhost:8443/';
    baseUrl:string = 'https://tag4it.com/';
    // baseUrl:string = 'https://localhost:443/'; 

    inverseProxy:string = 'https://cors-anywhere.herokuapp.com/';

    loadingGifUrl: string = 'assets/content/loading.gif';
    tagLogoUrl: string = 'assets/content/logo/tag-logo.png';
}