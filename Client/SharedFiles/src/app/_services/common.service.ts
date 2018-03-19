import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { AppConfig } from '../app.config';
import { Observable } from 'rxjs/Observable';
import { SelectItem } from 'primeng/primeng';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable()
export class CommonService extends BaseService {

    baseUrl: string = '';

    constructor(
        private http: HttpClient, 
        private _appConfig: AppConfig 
    ){
        super();
        this.baseUrl = _appConfig.baseUrl;
    }

    getCountryList() : Observable<SelectItem[]> {
        // return this.http.get<SelectItem[]>(this.baseUrl + 'api/commons/getCountryList');

        const headers = this.jwt_content_type_json().headers; 

        return this.http.get<SelectItem[]>(this.baseUrl + 'api/commons/getCountryList', { headers: headers });
    }

    getCountyList(countryId: number) : Observable<SelectItem[]> {
        // return this.http.get<SelectItem[]>(this.baseUrl + 'api/commons/getCountyList?countryId=' + countryId);

        const headers = this.jwt_content_type_json().headers;
        const params = new HttpParams().set('countryId', countryId.toString());

        return this.http.get<SelectItem[]>(this.baseUrl + 'api/commons/getCountyList', { headers: headers, params: params });
    }

    getCityList(countyId: number) : Observable<SelectItem[]> {
        // return this.http.get<SelectItem[]>(this.baseUrl + 'api/commons/getCityList?countyId=' + countyId);

        const headers = this.jwt_content_type_json().headers;
        const params = new HttpParams().set('countyId', countyId.toString());

        return this.http.get<SelectItem[]>(this.baseUrl + 'api/commons/getCityList', { headers: headers, params: params });
    }

}
