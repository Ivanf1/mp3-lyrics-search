import { useEffect, useState } from "react";
import { SOLR_URL } from "../constants/constants";
import SearchResult from "./SearchResult";

interface Highlighting {
  [key: string]: { lyrics: string[] };
}

const Search = () => {
  const [lyrics, setLyrics] = useState("");
  const [results, setResults] = useState<Highlighting | {} | null>(null);

  useEffect(() => {
    const searchLyrics = async (lyrics: string) => {
      const lyricsEncoded = encodeURIComponent(`(${lyrics.replace(" ", "&")})`);

      const data = await fetch(
        `${SOLR_URL}select?hl.fl=lyrics&hl=true&q.op=OR&q=lyrics%3A${lyricsEncoded}`
      );

      const json = await data.json();
      setResults(json.highlighting as Highlighting);
    };

    const timeOutId = setTimeout(() => {
      if (lyrics !== "") {
        searchLyrics(lyrics);
      } else {
        setResults(null);
      }
    }, 500);
    return () => clearTimeout(timeOutId);
  }, [lyrics]);

  let resultsRenderer = null;

  if (results) {
    if (Object.keys(results).length > 0) {
      resultsRenderer = Object.keys(results).map((keyName, i) => (
        <SearchResult
          title={keyName}
          lyrics={(results as Highlighting)[keyName].lyrics[0]}
          key={i}
        />
      ));
    } else {
      resultsRenderer = "no match found";
    }
  }

  return (
    <>
      <span className="section-header">Search</span>
      <div className="section-container">
        <div id="search-box-container">
          <textarea
            name="search-box"
            id="search-box"
            placeholder="Search lyrics"
            onChange={(e) => setLyrics(e.target.value.trim())}
          />
        </div>
        <div id="search-results-container">{resultsRenderer}</div>
      </div>
    </>
  );
};

export default Search;
