FROM node:24-bookworm-slim AS build
WORKDIR /app
COPY backend/package.json backend/package-lock.json ./
RUN npm ci --no-audit --no-fund
COPY backend/tsconfig.json ./
COPY backend/src ./src
RUN npm test && npm run build && npm prune --omit=dev
FROM node:24-bookworm-slim AS runtime
ENV NODE_ENV=production DATA_DIR=/data PORT=3000
WORKDIR /app
COPY --from=build --chown=node:node /app/node_modules ./node_modules
COPY --from=build --chown=node:node /app/dist ./dist
COPY --chown=node:node config/prices.json ./config/prices.json
RUN mkdir /data && chown node:node /data && chmod 700 /data
USER node
VOLUME ["/data"]
EXPOSE 3000
CMD ["node","dist/server.js"]
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 CMD node -e 'fetch("http://127.0.0.1:"+(process.env.PORT||3000)+"/health",{headers:{authorization:"Bearer "+process.env.BRIDGE_TOKEN}}).then(r=>{if(!r.ok)process.exit(1)}).catch(()=>process.exit(1))'
