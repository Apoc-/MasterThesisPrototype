using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class WikiScreenBehaviour : MonoBehaviour, IPointerClickHandler
    {
        private Dictionary<string, WikiEntryData> _data;

        public string WikiPath = "Wiki/Scrum";
        public string MainEntryId = "Scrum";

        public WikiEntry WikiEntryPrefab;
        public GameObject RootGo;
        private WikiEntry CurrentDisplayWikiEntry;

        private Stack<string> _visitedPages = new Stack<string>();
        private Dictionary<string, WikiEntry> _createdEntries = new Dictionary<string, WikiEntry>();

        private void Awake()
        {
            LoadWikiData();
        }

        private void OnEnable()
        {
            DisplayMainWikiEntry();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var linkIndex =
                TMP_TextUtilities.FindIntersectingLink(CurrentDisplayWikiEntry.Body, Input.mousePosition, null);
            if (linkIndex != -1)
            {
                var linkInfo = CurrentDisplayWikiEntry.Body.textInfo.linkInfo[linkIndex];

                _visitedPages.Push(CurrentDisplayWikiEntry.Id);
                DisplayWikiEntryById(linkInfo.GetLinkID());
            }
        }


        public void GoBack()
        {
            string id = "";
            if (_visitedPages.Count > 0)
            {
                id = _visitedPages.Pop();
                DisplayWikiEntryById(id);
            }
        }

        public void DisplayMainWikiEntry()
        {
            if (CurrentDisplayWikiEntry != null)
            {
                _visitedPages.Push(CurrentDisplayWikiEntry.Id);
            }
            
            DisplayWikiEntryById(MainEntryId);   
        }

        public void DisplayWikiEntryById(string id)
        {
            if (!_data.ContainsKey(id))
            {
                Debug.LogError("Key " + id + " not found in WikiEntry Data. Wiki: " + WikiPath);
            }
            
            HideAllEntries();

            WikiEntry entry;
            
            if(_createdEntries.ContainsKey(id))
            {
                entry = _createdEntries[id];
            }
            else
            {
                var data = _data[id];
                entry = Instantiate(WikiEntryPrefab, RootGo.transform, false);
                entry.Title.text = data.Title;
                entry.Body.text = data.BodyText;
                entry.Id = id;
                HandleDataLinksInEntry(entry);
                _createdEntries[id] = entry;
            }
            
            entry.gameObject.SetActive(true);
            CurrentDisplayWikiEntry = entry;
        }

        private void HideAllEntries()
        {
            _createdEntries.Values.ToList().ForEach(entry => entry.gameObject.SetActive(false));
        }

        private void HandleDataLinksInEntry(WikiEntry entry)
        {
            var links = ParseLinks(entry);
            var legalTemplate = "<link=\"{id}\"><color=\"blue\">{id}</color></link>";
            var text = entry.Body.text;
            links.ForEach(link =>
            {
                var id = link.Value.Trim('[').Trim(']');
                var linkedText = legalTemplate.Replace("{id}", id);
                text = text.Replace(link.Value, linkedText);
            });
            entry.Body.text = text;
        }

        private List<KeyValuePair<int, string>> ParseLinks(WikiEntry entry)
        {
            var text = entry.Body.text;
            var opened = false;
            var link = "";
            var pos = -1;

            var parsedLinks = new List<KeyValuePair<int, string>>();
            for (var i = 0;
                i < text.Length;
                i++)
            {
                if (text[i] == '[')
                {
                    opened = true;
                    pos = i;
                }

                if (opened)
                {
                    link += text[i];
                }

                if (text[i] == ']')
                {
                    opened = false;
                    parsedLinks.Add(new KeyValuePair<int, string>(pos, link));
                    pos = -1;
                    link = "";
                }
            }

            return parsedLinks;
        }

        private void LoadWikiData()
        {
            _data = new Dictionary<string, WikiEntryData>();
            var rawData = Resources.LoadAll<TextAsset>(WikiPath);
            rawData.ToList().ForEach(textAsset =>
            {
                var wikiEntryData = new WikiEntryData();
                var lines = textAsset.text.Split('\n').ToList();

                lines.ForEach(line =>
                {
                    if (line.StartsWith("$"))
                    {
                        HandleCommand(line, wikiEntryData);
                    }
                    else
                    {
                        wikiEntryData.BodyText += line + "\n";
                    }
                });

                try
                {
                    _data.Add(wikiEntryData.Title, wikiEntryData);
                }
                catch (ArgumentNullException ex)
                {
                    Debug.LogError("Tried to add a Wiki entry without a defined title. " +
                                   "(Text Asset: " + textAsset.name + ")");
                    Debug.LogError(ex);
                }
            });
        }

        private void HandleCommand(string line, WikiEntryData wikiEntryData)
        {
            var token = line.Trim().Trim('$').Split('=');
            var cmd = token[0].ToLower();

            var arg = token[1];
            switch (cmd)
            {
                case "title":
                    wikiEntryData.Title = arg;
                    break;
                default:
                    return;
            }
        }
    }
}