import { Photo } from "./photo";

export interface Member {
    id: number;
    userName: string;
    knownAs: string;
    photoUrl: string;
    age: number;
    created: Date;
    lastActive: Date;
    introduction: string;
    interests: string;
    city: string;
    country: string;
    photo: Photo
}

