export class Game {
    constructor(){
        
    }

    private _Name: string;
    public get Name(): string {
        return this._Name;
    }
    public set Name(v: string) {
        this._Name = v;
    }

    private _ID: number;
    public get ID(): number {
        return this._ID;
    }

    public set ID(v: number) {
        this._ID = v;
    }

    private _Price: number;
    public get Price(): number {
        return this._Price;
    }

    public set Price(v: number) {
        this._Price = v;
    }

    private _ImgPath: string;
    public get ImgPath(): string {
        return this._ImgPath;
    }
    public set ImgPath(v: string) {
        this._ImgPath = v;
    }
}