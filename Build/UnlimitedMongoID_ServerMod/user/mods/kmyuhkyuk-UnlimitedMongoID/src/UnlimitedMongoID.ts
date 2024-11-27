import { DependencyContainer } from "tsyringe";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";
import { DynamicRouterModService } from "@spt/services/mod/dynamicRouter/DynamicRouterModService";
import { StaticRouterModService } from "@spt/services/mod/staticRouter/StaticRouterModService";
import { HashUtil } from "@spt/utils/HashUtil";

class UnlimitedMongoID implements IPreSptLoadMod
{
    private loadClientModRecord: Record<string, boolean> = {};

    public preSptLoad(container: DependencyContainer): void 
    {
        const logger = container.resolve<ILogger>("WinstonLogger");
        const dynamicRouterModService = container.resolve<DynamicRouterModService>("DynamicRouterModService");
        const staticRouterModService = container.resolve<StaticRouterModService>("StaticRouterModService");

        container.afterResolution("HashUtil", (_t, result: HashUtil) =>
        {
            result.isValidMongoId = () =>
            {
                return true;
            }
        }, { frequency: "Always" });

        logger.warning("Server MongoID verification has been unlocked, Any MongoID errors have nothing to do with SPT.");

        dynamicRouterModService.registerDynamicRouter(
            "DynamicUnlimitedMongoIDRoute",
            [
                {
                    url: "/unlimited-mongoid/client",
                    action: async (url, info, sessionId) =>
                    {
                        this.loadClientModRecord[sessionId] = true;

                        return JSON.stringify({ response: "OK" });
                    }
                }
            ],
            "unlimited-mongoid"
        );

        staticRouterModService.registerStaticRouter(
            "StaticUnlimitedMongoIDRoute",
            [
                {
                    url: "/client/game/start",
                    action: async (url, info, sessionId, output) =>
                    {
                        if (this.loadClientModRecord[sessionId])
                        {
                            this.loadClientModRecord[sessionId] = false;
                        }
                        else
                        {
                            logger.error(`${sessionId} Unlimited MongoID Client Mod not loaded`);
                        }

                        return output;
                    }
                }
            ],
            "unlimited-mongoid"
        );
    }
}

module.exports = { mod: new UnlimitedMongoID() }