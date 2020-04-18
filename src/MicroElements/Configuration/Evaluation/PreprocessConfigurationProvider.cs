﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace MicroElements.Configuration.Evaluation
{
    /// <summary>
    /// Провайдер конфигурации для препроцессинга конфигурации.
    /// </summary>
    public class PreprocessConfigurationProvider : FileConfigurationProvider
    {
        private readonly FileConfigurationProvider _configurationProvider;
        private readonly string _rootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreprocessConfigurationProvider"/> class.
        /// </summary>
        /// <param name="configurationProvider">configurationProvider</param>
        /// <param name="rootPath">rootPath</param>
        public PreprocessConfigurationProvider(FileConfigurationProvider configurationProvider, string rootPath)
            : base(configurationProvider.Source)
        {
            _configurationProvider = configurationProvider;
            _rootPath = rootPath;
        }

        /// <inheritdoc />
        public override void Load(Stream stream)
        {
            _configurationProvider.Load(stream);

            // Получим список ключей
            var keys = _configurationProvider.GetKeys();

            // ${include}, ${include}:0, ...
            bool IsIncludeKey(string key) => key.StartsWith("${include}");

            foreach (string key in keys)
            {
                if (IsIncludeKey(key))
                {
                    if (_configurationProvider.TryGet(key, out string includePath))
                    {
                        LoadIncludedConfiguration(includePath, Data);
                    }
                }
                else
                {
                    _configurationProvider.CopyValueToDictionary(key, Data);
                }
            }
        }

        private void LoadIncludedConfiguration(string includePath, IDictionary<string, string> targetDictionary)
        {
            var path = Path.Combine(_rootPath, includePath);
            var fullPath = Path.GetFullPath(path);

            // Создадим провайдер конфигурации и загрузим значения из него
            var jsonConfigurationProvider = CreateConfigurationProvider(fullPath);
            jsonConfigurationProvider.Load();

            // Получим все ключи
            var keysToInclude = jsonConfigurationProvider.GetKeys();

            // Добавим все данные из подгруженного файла
            jsonConfigurationProvider.CopyValuesToDictionary(keysToInclude, targetDictionary);
        }

        private static IConfigurationProvider CreateConfigurationProvider(string fullPath)
        {
            // todo: Можно расширить виды поддерживаемых провайдеров
            var jsonConfigurationSource = new JsonConfigurationSource { Path = fullPath };
            jsonConfigurationSource.ResolveFileProvider();
            var jsonConfigurationProvider = new JsonConfigurationProvider(jsonConfigurationSource);
            return jsonConfigurationProvider;
        }
    }
}
