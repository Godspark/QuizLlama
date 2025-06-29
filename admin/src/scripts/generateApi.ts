import * as path from "node:path";
import * as process from "node:process";
import { generateApi } from "swagger-typescript-api";

await generateApi({ input: "http://100.105.115.119:5114/swagger/v1/swagger.json" });
