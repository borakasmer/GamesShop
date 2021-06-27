import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Game } from 'src/models/game';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class GameService {
    gameUrl = "http://localhost:42213/games/";
    constructor(private httpClient: HttpClient) { }

    GetGames(connectionId: string): Promise<any[]> {
        return this.httpClient.get<Game[]>(this.gameUrl+ `${connectionId}`).toPromise();
    }
}