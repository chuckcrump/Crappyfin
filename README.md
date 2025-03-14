# CrappyFin
### Your home media platform.
> [!Caution]
> Please don't use this for your personal media yet.

This is not realated to Jellyfin in any way.

### Routes
* `/` is the first landing page it dosen't work right now.
* `/login` is a placeholder login page.
* `/media` will be the place to see all your media folders.
* `/media/movies` is the only fully functional route.

### How to run
* Clone the repo `git clone https://github.com/chuckcrump/Crappyfin`
* Install packages `npm install` or `bun install`
* You need a `/media` folder in the root project or point the `server.ts` to another path.
* Also create a `.env` file to point at the server urls for the typescript and go backend.
* finally run `server.ts` and `bun start` or `npm start`

### Built with
* SvelteKit
* Hono
* Go
  
C# backend is in progress.
