class ArticleDashboard0 extends HTMLElement {

    connectedCallback() {

        this.innerHTML = `
            <div style="padding: 30px;">
                <h1>Article Dashboard</h1>

                <p>
                    Your custom Umbraco 17 Dashboard
                    is working!
                </p>
            </div>
        `;
    }
}




class ArticleDashboard1 extends HTMLElement {

    constructor() {
        super();

        this.attachShadow({ mode: "open" });
    }

    connectedCallback() {
        this.render();
        this.loadStatistics();
    }

    async loadStatistics() {

        try {
            const response = await fetch(
                "/umbraco/management/api/v1/article-dashboard/statistics",
                {
                    credentials: "same-origin"
                }
            );

            if (!response.ok) {
                throw new Error("Unable to load statistics");
            }

            const data = await response.json();

            this.shadowRoot.querySelector(
                "#total"
            ).textContent = data.totalArticles;

            this.shadowRoot.querySelector(
                "#published"
            ).textContent = data.publishedArticles;

        } catch (error) {

            this.shadowRoot.querySelector(
                "#error"
            ).textContent = error.message;

        }
    }

    render() {

        this.shadowRoot.innerHTML = `
            <style>
                :host {
                    display: block;
                    padding: 24px;
                }

                .container {
                    display: flex;
                    gap: 20px;
                    flex-wrap: wrap;
                }

                .card {
                    background: white;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    padding: 24px;
                    min-width: 200px;
                }

                h2 {
                    margin-top: 0;
                }

                .number {
                    font-size: 32px;
                    font-weight: bold;
                }

                button {
                    padding: 10px 18px;
                    cursor: pointer;
                }
            </style>

            <h1>Article Dashboard</h1>

            <p>Overview of your website articles.</p>

            <div class="container">

                <div class="card">
                    <h2>Total Articles</h2>
                    <div class="number" id="total">Loading...</div>
                </div>

                <div class="card">
                    <h2>Published Articles</h2>
                    <div class="number" id="published">
                        Loading...
                    </div>
                </div>

            </div>

            <br />

            <button id="refresh">
                Refresh Statistics
            </button>

            <p id="error" style="color: red;"></p>
        `;

        this.shadowRoot
            .querySelector("#refresh")
            .addEventListener("click", () => {
                this.loadStatistics();
            });
    }
}

import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { html } from "@umbraco-cms/backoffice/external/lit";

class ArticleDashboard3 extends UmbLitElement {

    constructor() {
        super();

        this.totalArticles = 0;
        this.publishedArticles = 0;
        this.errorMessage = "";
    }

    async connectedCallback() {
        super.connectedCallback();

        this.authContext = await this.getContext(
            UMB_AUTH_CONTEXT
        );

        await this.loadStatistics();
    }

    async loadStatistics() {

        try {

            this.errorMessage = "";

            const config =
                this.authContext.getOpenApiConfiguration();

            const token = await config.token();

            const response = await fetch(
                "/umbraco/management/api/v1/article-dashboard/statistics",
                {
                    method: "GET",
                    credentials: "include",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json"
                    }
                }
            );

            if (!response.ok) {
                throw new Error(
                    `API request failed: ${response.status}`
                );
            }

            const data = await response.json();

            this.totalArticles = data.totalArticles;
            this.publishedArticles = data.publishedArticles;

        } catch (error) {

            console.error(
                "Article Dashboard Error:",
                error
            );

            this.errorMessage = error.message;

        }

        this.requestUpdate();
    }

    render() {

        return html`
            <uui-box headline="Article Dashboard">

                <h2>Article Statistics</h2>

                <div style="display:flex; gap:20px;">

                    <uui-box>
                        <h3>Total Articles</h3>
                        <h1>${this.totalArticles}</h1>
                    </uui-box>

                    <uui-box>
                        <h3>Published Articles</h3>
                        <h1>${this.publishedArticles}</h1>
                    </uui-box>

                </div>

                <br />

                <uui-button
                    look="primary"
                    label="Refresh Statistics"
                    @click=${this.loadStatistics}>
                    Refresh Statistics
                </uui-button>

                ${this.errorMessage
                ? html`<p>${this.errorMessage}</p>`
                : ""
            }

            </uui-box>
        `;
    }
}


class ArticleDashboard4 extends HTMLElement {

    async connectedCallback() {
        this.innerHTML = `
            <div style="padding: 24px;">
                <h1>Article Dashboard</h1>

                <div id="stats">
                    Loading article statistics...
                </div>
            </div>
        `;

        await this.loadStats();
    }

    async loadStats() {
        const container = this.querySelector("#stats");

        try {
            const response = await fetch(
                "/umbraco/api/dashboard/statistics",
                {
                    credentials: "same-origin"
                }
            );

            if (!response.ok) {
                throw new Error("Unable to load statistics");
            }

            const data = await response.json();

            container.innerHTML = `
                <h2>Total Articles: ${data.totalArticles}</h2>
                <h2>Published Articles: ${data.publishedArticles}</h2>
            `;

        } catch (error) {
            container.textContent =
                "Unable to load article statistics.";
            console.error(error);
        }
    }
}



customElements.define("article-dashboard", ArticleDashboard3);
