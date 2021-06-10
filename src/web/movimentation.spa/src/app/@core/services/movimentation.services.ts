import { Movimentation } from './../models/movimentation';
import { Injectable } from "@angular/core";
import { of as observableOf, Observable } from "rxjs";
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable()
export class MovimentationService {

  protected url: string = `${environment.url}api/movimentations`;

  constructor(private http: HttpClient) { }

  getMovimentations(): Observable<any[]> {
    return this.http.get<any[]>(this.url);
  }

  createMovimentation(movimentation: Movimentation): Observable<any> {
    return this.http.post<any>(this.url, movimentation);
  }

  updateMovimentation(movimentation: Movimentation): Observable<any> {
    return this.http.put<any>(this.url, movimentation);
  }

  deleteMovimentation(id: string): Observable<any> {
    return this.http.delete<any>(`${this.url}?id=${id}`);
  }
}