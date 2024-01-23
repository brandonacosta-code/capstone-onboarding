// @ts-check

import fs from "fs";
import path from "path";

const tsconfig = fs.readFileSync('./tsconfig.json', 'utf8');

var dirPath = path.resolve(tsconfig.match(/"outDir": "(.*)"/)?.[1] ?? '');

fs.rmSync(dirPath, { recursive: true, force: true });

console.log(`Deleted ${dirPath}`)