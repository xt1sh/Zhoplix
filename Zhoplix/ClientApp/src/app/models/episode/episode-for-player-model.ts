import { VideoForPlayer } from "../video/video-for-player";

export interface EpisodeForPlayerModel {
    thumbnailsAmount: number;
    name: string;
    videos: Array<VideoForPlayer>;
    location: string;
    thumbnailLocation: string;
}