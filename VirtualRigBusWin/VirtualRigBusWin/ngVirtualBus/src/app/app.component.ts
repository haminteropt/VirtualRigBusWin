import { DirectoryList } from './Model/directory-list';
import { Component, OnInit } from '@angular/core';
import { DirService, UdpCmdPacket } from './services/DirService.service';
import { ConfigService } from './services/config-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit  {
  title = 'app';
  public serviceList: DirectoryList = new DirectoryList();
  constructor(private dirSrv: DirService, private configSrv: ConfigService) { }
  ngOnInit() {
    this.dirSrv.getList().subscribe(val => {
      if (!val || val.length === 0) return;
      this.serviceList = val;
      this.configSrv.virtualBusUrl = this.getVirtualRigUrl();
      console.log(val);
      console.log(this.configSrv.virtualBusUrl);
    });
  }
  getVirtualRigUrl(): string {
    const rigInfo = this.serviceList.rigBuses.find((item) => {
      if (item.name === 'VirtualRig') return true;
      else return false;
    });
    return `http://${rigInfo.host}:${rigInfo.tcpPort}`;
  }

}
