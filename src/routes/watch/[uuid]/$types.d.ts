import type * as Kit from "@sveltejs/kit";

type RouteParams = {
  uuid: string;
};

export type PageServerLoad = Kit.Load<RouteParams>;
