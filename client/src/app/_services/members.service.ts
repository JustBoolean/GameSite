import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  constructor(private http: HttpClient) { 
    
  }

  getMembers(page?: number, itemsPerPage?:number) {
    // if(this.members.length > 0) {
    //   return of(this.members);
    // }
    // return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
    //   map(members => {
    //     this.members = members;
    //     return members;  
    //   })
    // );
    let params = new HttpParams();
    if(page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber',page.toString());
      params = params.append('pageSize',itemsPerPage.toString());
    }

    return this.http.get<Member[]>(this.baseUrl+'users', {observe: 'response', params}).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if(response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult; 
      })
    )
  }

  getMember(username : string) {
    // const member = this.members.find(u => u.userName === username);
    // if(member !== undefined) {
    //   return of(member);
    // }
    // return this.http.get<Member>(this.baseUrl + 'users/' + username);
    return this.http.get<Member>(this.baseUrl+'users/'+username);
  }

  updateMember(member: Member){
    // return this.http.put(this.baseUrl + 'users', member).pipe(
    //   map(() => {
    //     const index = this.members.indexOf(member);
    //     this.members[index] = member;
    //   })
    // );
    return this.http.put(this.baseUrl+'users',member);
  }

  setPhoto(file: File) {
    return this.http.post(this.baseUrl + 'users/add-photo', {file})
  }

  addFollow(username: string) {
    return this.http.post(this.baseUrl + 'follows/' + username, {});
  }

  getFollows(predicate: string, pageNumber: number, pageSize: number) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'follows', params, this.http);
  }


}
