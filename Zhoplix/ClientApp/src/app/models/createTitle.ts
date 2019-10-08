import { CreateSeason } from "./createSeason";

export class CreateTitle {
  name: string = '';
  description: string = '';
  ageRestriction: number = 0;
  seasons: Array<CreateSeason> = new Array<CreateSeason>();
}
