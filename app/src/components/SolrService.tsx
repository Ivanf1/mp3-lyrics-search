import { useEffect, useState } from "react";
import { SOLR_URL } from "../constants/constants";

enum Status {
  Loading = "loading",
  Starting = "starting",
  NotRunning = "not running",
  Running = "running",
}

const SolrService = () => {
  const [status, setStatus] = useState<Status>(Status.Loading);

  useEffect(() => {
    const checkStatus = async () => {
      fetch(`${SOLR_URL}select`)
        .then((response) => {
          if (response.status === 503) {
            setStatus(Status.Starting);
          } else if (response.ok) {
            setStatus(Status.Running);
          }
        })
        .catch((_) => {
          setStatus(Status.NotRunning);
        });
    };

    checkStatus();
    let timerId = setInterval(() => checkStatus(), 5000);
    return () => clearInterval(timerId);
  }, []);

  return (
    <div className="service-status-container">
      <div className="service-name">Solr</div>
      <div className="service-status">{status}</div>
    </div>
  );
};

export default SolrService;
