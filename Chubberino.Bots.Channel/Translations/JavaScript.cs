using System;

namespace Chubberino.Bots.Channel.Translations
{
    public static class JavaScript
    {
        public const String Translate = @"
const translate = require('@vitalets/google-translate-api');

module.exports = async (message) => {
    try {
        const res = await translate(message, { to: 'en', client: 'gtx' });
        return res.text;
    } catch (err) {
        return null;
    }
}";

    }
}
