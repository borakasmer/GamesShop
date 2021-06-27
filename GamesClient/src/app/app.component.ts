import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from 'src/models/game';
import { GameService } from 'src/services/gameService';

import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Games';
  modelGames: Array<Game>;

  _hubConnection: HubConnection;
  _connectionId: string;
  signalRServiceIp: string = "http://localhost:42213/gameHub";

  public constructor(private service: GameService) {
    this.modelGames = new Array<Game>();
  }

  public ngOnInit(): void { 
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.signalRServiceIp}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this._hubConnection.start().then(
      () => console.log("Hub Connection Start"))
      .catch(err => console.log(err));

    this._hubConnection.on('GetConnectionId', (connectionId: string) => {
      this._connectionId = connectionId;
      console.log("ConnectionID :" + connectionId);
      this.service.GetGames(connectionId).then(res => {
        this.modelGames = res;
      });
    });

    this._hubConnection.on('ChangeGame', (game: Game) => {
      //console.log("Updated Game:" + JSON.stringify(game));
      //console.log("Game Data Push:"+JSON.stringify(this.modelGames));
      var item = this.modelGames.find(rd => rd.Name == game.Name);
      //console.log("Current Game:" + JSON.stringify(item));
      this.modelGames = this.modelGames.filter(gam => gam != item);            
      this.modelGames.push(game);
      //console.log("Row Data Push:" + JSON.stringify(this.modelGames));
    });
  }
}
