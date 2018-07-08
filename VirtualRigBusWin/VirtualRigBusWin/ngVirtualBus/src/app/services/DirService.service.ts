import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

export class UdpCmdPacket {
  public docType: string;
  public id: string;
  public command: string;
  public time: number;
  public name: string;
  public description: string;
  public host: string;
  public ip: string;
  public udpPort: number;
  public tcpPort: number;
  public minVersion: number;
  public maxVersion: number;
}

export class RigBusInfo extends UdpCmdPacket {
  public RigType: string;
  public SendSyncInfo: boolean;
  public HonorTx: boolean;
}
@Injectable({
  providedIn: 'root'
})
export class DirService {

  constructor(private http: HttpClient) { }
  public getList(): Observable<any> {
    return this.http.get('http://localhost:7300/api/Directory/V1/list');
  }
  private commonRequest(content: any, url: string, method: string = 'put'): Observable<any> {

    const reqHeader = new HttpHeaders().
      set('Content-Type', 'application/json');



    if (content) {
      const contentJson: string = JSON.stringify(content);
      switch (method.toLowerCase()) {
        case 'put':
          if (content && content._rev && content._rev.length >= 5) {
            url = url + '/' + content._id;
          }
          return this.http.put(url, contentJson, { headers: reqHeader });

        case 'get':
          return this.http.get(url, { headers: reqHeader });
        case 'delete':
          return this.http.delete(url, { headers: reqHeader });
      }
    }
  }
}
