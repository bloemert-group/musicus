/* === dont forget to import scss to main.js file === */
/* ===> import './main.scss'; <=== */

var path = require("path");

module.exports = {
	entry: "./Scripts/index.jsx",
	output: {
		path: path.resolve(__dirname, "wwwroot"),
		filename: "js/bundle.js",
		publicPath: "/"
	},
	module: {
		rules: [
			{
				test: /\.jsx$/,
				include: path.resolve(__dirname, "Scripts"),
				use: {
					loader: "babel-loader",
					options: { presets: ['es2015', 'react'] }
				}
			},
			{
				test: /\.scss$/,
				use: [
					{
						loader: "style-loader" // creates style nodes from JS strings
					},
					{
						loader: "css-loader" // translates CSS into CommonJS
					},
					{
						loader: "sass-loader" // compiles Sass to CSS
					}
				]
			},
			{
				test: /\.css$/,
				loader: "style-loader!css-loader"
			},
			{
				test: /\.svg$/,
				use: {
					loader: 'url-loader?limit=65000&mimetype=image/svg+xml&name=public/fonts/[name].[ext]'
				}
			},
			{
				test: /\.woff$/,
				use: {
					loader: 'url-loader?limit=65000&mimetype=application/font-woff&name=public/fonts/[name].[ext]'
				}
			},
			{
				test: /\.woff2$/,
				use: {
					loader: 'url-loader?limit=65000&mimetype=application/font-woff2&name=public/fonts/[name].[ext]'
				}
			},
			{
				test: /\.[ot]tf$/,
				use: {
					loader: 'url-loader?limit=65000&mimetype=application/octet-stream&name=public/fonts/[name].[ext]'
				}
			},
			{
				test: /\.eot$/,
				use: {
					loader: 'url-loader?limit=65000&mimetype=application/vnd.ms-fontobject&name=public/fonts/[name].[ext]'
				}
			}
		]
	}
};