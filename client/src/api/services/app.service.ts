import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {FieldChoice} from "../models/fieldChoice";


@Injectable({
  providedIn: 'root'
})
export class AppService {
  constructor(protected httpClient: HttpClient) { }

  getAutoComplete(table: string, substring: string = '', limit: number = 10): Observable<Array<FieldChoice>> {
    return this.httpClient.get<Array<FieldChoice>>( environment.host + `${table}/autocomplete?substring=${substring}&limit=${limit}`);
  }
}
