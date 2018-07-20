import { Component, OnInit } from '@angular/core';
import { DirService, UdpCmdPacket } from './services/DirService.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit  {
  title = 'app';
  public serviceList: Array<UdpCmdPacket> = new Array<UdpCmdPacket>();
  constructor(private dirSrv: DirService) { }
  ngOnInit() {
    this.dirSrv.getList().subscribe(val => {
      if (!val || val.length === 0) return;
      this.serviceList = val;
      console.log(val);
    });
  }

}
